#include "pch.h"
#include "DeviceDriverManager.h"
#include <string>
#include <vector>
#include <newdev.h> // Include the header for UpdateDriverForPlugAndPlayDevices

#pragma comment(lib, "setupapi.lib")
#pragma comment(lib, "newdev.lib")


std::vector<std::wstring> ListDeviceProperties(const std::wstring& deviceId);
std::vector<std::wstring> GetDeviceProperty(HDEVINFO deviceInfoSet, SP_DEVINFO_DATA& deviceInfoData, DWORD property);


extern "C" __declspec(dllexport) bool InstallDriver(const wchar_t* deviceId, const wchar_t* infPath, DeviceInfo* uninstallData)
{
    std::wstring deviceIdStr(deviceId);
    std::wstring infPathStr(infPath);

    auto deviceIds = ListDeviceProperties(deviceIdStr);
    if (deviceIds.empty()) {
        return false;
    }

    for (const auto& id : deviceIds) {
        BOOL reboot = FALSE;
        if (UpdateDriverForPlugAndPlayDevices(NULL, id.c_str(), infPathStr.c_str(), INSTALLFLAG_FORCE, &reboot)) {
            wcsncpy_s(uninstallData->deviceId, id.c_str(), _TRUNCATE);

            if (GetDeviceInfo(id.c_str(), uninstallData)) {
                return true;
            }
        }
    }

    return false;
}
std::vector<std::wstring> ListDeviceProperties(const std::wstring& deviceId) {
    std::vector<std::wstring> deviceIds;
    HDEVINFO deviceInfoSet = SetupDiGetClassDevs(NULL, deviceId.c_str(), NULL, DIGCF_PRESENT | DIGCF_ALLCLASSES);
    if (deviceInfoSet == INVALID_HANDLE_VALUE) {
        return deviceIds;
    }

    SP_DEVINFO_DATA deviceInfoData;
    deviceInfoData.cbSize = sizeof(SP_DEVINFO_DATA);

    if (!SetupDiEnumDeviceInfo(deviceInfoSet, 0, &deviceInfoData)) {
        SetupDiDestroyDeviceInfoList(deviceInfoSet);
        return deviceIds;
    }

    auto hardwareIds = GetDeviceProperty(deviceInfoSet, deviceInfoData, SPDRP_HARDWAREID);
    auto compatibleIds = GetDeviceProperty(deviceInfoSet, deviceInfoData, SPDRP_COMPATIBLEIDS);

    deviceIds.insert(deviceIds.end(), hardwareIds.begin(), hardwareIds.end());
    deviceIds.insert(deviceIds.end(), compatibleIds.begin(), compatibleIds.end());

    SetupDiDestroyDeviceInfoList(deviceInfoSet);
    return deviceIds;
}

std::vector<std::wstring> GetDeviceProperty(HDEVINFO deviceInfoSet, SP_DEVINFO_DATA& deviceInfoData, DWORD property) {
    std::vector<std::wstring> deviceProperties;
    DWORD requiredSize = 0;
    SetupDiGetDeviceRegistryProperty(deviceInfoSet, &deviceInfoData, property, NULL, NULL, 0, &requiredSize);

    if (GetLastError() != ERROR_INSUFFICIENT_BUFFER) {
        return deviceProperties;
    }

    std::vector<BYTE> buffer(requiredSize);
    if (SetupDiGetDeviceRegistryProperty(deviceInfoSet, &deviceInfoData, property, NULL, buffer.data(), static_cast<DWORD>(buffer.size()), NULL)) {
        const WCHAR* deviceProperty = reinterpret_cast<const WCHAR*>(buffer.data());
        while (*deviceProperty) {
            deviceProperties.push_back(deviceProperty);
            deviceProperty += wcslen(deviceProperty) + 1;
        }
    }

    return deviceProperties;
}


extern "C" __declspec(dllexport) bool GetDeviceInfo(const wchar_t* deviceId, DeviceInfo* deviceInfo)
{
    std::wstring deviceIdStr(deviceId);
    deviceInfo->deviceInfoSet = SetupDiGetClassDevs(NULL, deviceIdStr.c_str(), NULL, DIGCF_PRESENT | DIGCF_ALLCLASSES);
    if (deviceInfo->deviceInfoSet == INVALID_HANDLE_VALUE) {
        return false;
    }

    deviceInfo->deviceInfoData.cbSize = sizeof(SP_DEVINFO_DATA);
    if (!SetupDiEnumDeviceInfo(deviceInfo->deviceInfoSet, 0, &deviceInfo->deviceInfoData)) {
        SetupDiDestroyDeviceInfoList(deviceInfo->deviceInfoSet);
        deviceInfo->deviceInfoSet = INVALID_HANDLE_VALUE;
        return false;
    }

    wcsncpy_s(deviceInfo->deviceId, 256, deviceId, _TRUNCATE);
    return true;
}
extern "C" __declspec(dllexport) bool UninstallDriver(const wchar_t* deviceId, DeviceInfo* uninstallData)
{
    DeviceInfo deviceInfo;
    if (!GetDeviceInfo(deviceId, &deviceInfo))
    {
        // If the device is not present, use the provided uninstallData
        deviceInfo = *uninstallData;
    }

    if (!SetupDiCallClassInstaller(DIF_REMOVE, deviceInfo.deviceInfoSet, const_cast<SP_DEVINFO_DATA*>(&deviceInfo.deviceInfoData))) {
        DWORD error = GetLastError();
        std::wcerr << L"Failed to call class installer for removal. Error code: " << error << L"\n";
        SetupDiDestroyDeviceInfoList(deviceInfo.deviceInfoSet);
        return false;
    }

    SetupDiDestroyDeviceInfoList(deviceInfo.deviceInfoSet);
    return true;
}

extern "C" __declspec(dllexport) const wchar_t* GetParentHardwareID(const wchar_t* instanceId) {
    static std::wstring result; // static to persist after function return

    HDEVINFO deviceInfoSet = SetupDiGetClassDevs(NULL, NULL, NULL, DIGCF_PRESENT | DIGCF_ALLCLASSES);
    if (deviceInfoSet == INVALID_HANDLE_VALUE) {
        std::cerr << "SetupDiGetClassDevs failed." << std::endl;
        return L"";
    }

    SP_DEVINFO_DATA deviceInfoData;
    deviceInfoData.cbSize = sizeof(SP_DEVINFO_DATA);

    for (DWORD i = 0; SetupDiEnumDeviceInfo(deviceInfoSet, i, &deviceInfoData); ++i) {
        // Get the device ID
        TCHAR deviceID[MAX_DEVICE_ID_LEN];
        if (CM_Get_Device_ID(deviceInfoData.DevInst, deviceID, MAX_DEVICE_ID_LEN, 0) != CR_SUCCESS) {
            continue;
        }

        if (wcscmp(deviceID, instanceId) == 0) {
            DEVINST parentDevInst;
            if (CM_Get_Parent(&parentDevInst, deviceInfoData.DevInst, 0) != CR_SUCCESS) {
                std::cerr << "CM_Get_Parent failed." << std::endl;
                SetupDiDestroyDeviceInfoList(deviceInfoSet);
                return L"";
            }

            TCHAR parentDeviceID[MAX_DEVICE_ID_LEN];
            if (CM_Get_Device_ID(parentDevInst, parentDeviceID, MAX_DEVICE_ID_LEN, 0) != CR_SUCCESS) {
                std::cerr << "CM_Get_Device_ID failed." << std::endl;
                SetupDiDestroyDeviceInfoList(deviceInfoSet);
                return L"";
            }

            // Find the USB\VID_xxx&PID_yyyy part of the parent device ID
            std::wstring parentDeviceIDStr(parentDeviceID);
            size_t vidPos = parentDeviceIDStr.find(L"USB\\VID_");
            if (vidPos != std::wstring::npos) {
                size_t pidPos = parentDeviceIDStr.find(L"&PID_", vidPos);
                if (pidPos != std::wstring::npos) {
                    size_t endPos = parentDeviceIDStr.find(L'&', pidPos + 5);
                    if (endPos != std::wstring::npos) {
                        result = parentDeviceIDStr.substr(vidPos, endPos - vidPos);
                        SetupDiDestroyDeviceInfoList(deviceInfoSet);
                        return result.c_str();
                    } else {
                        result = parentDeviceIDStr.substr(vidPos);
                        SetupDiDestroyDeviceInfoList(deviceInfoSet);
                        return result.c_str();
                    }
                }
            }

            std::cerr << "Parent DeviceID does not contain USB\\VID_ and PID_" << std::endl;
            SetupDiDestroyDeviceInfoList(deviceInfoSet);
            return L"";
        }
    }

    std::cerr << "Device not found." << std::endl;
    SetupDiDestroyDeviceInfoList(deviceInfoSet);
    return L"";
}
