

#pragma once

#include <windows.h>
#include <setupapi.h>

struct DeviceInfo {
    wchar_t deviceId[256];
    SP_DEVINFO_DATA deviceInfoData;
    HDEVINFO deviceInfoSet;
};
/*
extern "C" {


    extern "C" __declspec(dllexport) bool InstallDriver(const wchar_t* deviceId, const wchar_t* infPath, DeviceInfo* uninstallData);
    extern "C" __declspec(dllexport) bool GetDeviceInfo(const wchar_t* deviceId, DeviceInfo* deviceInfo);
    extern "C" __declspec(dllexport) bool UninstallDriver(const wchar_t* deviceId, const DeviceInfo* uninstallData);

}*/

#ifdef __cplusplus
extern "C" {
#endif

    __declspec(dllexport) bool InstallDriver(const wchar_t* deviceId, const wchar_t* infPath, DeviceInfo* uninstallData);
    __declspec(dllexport) bool GetDeviceInfo(const wchar_t* deviceId, DeviceInfo* deviceInfo);
    __declspec(dllexport) bool UninstallDriver(const wchar_t* deviceId, DeviceInfo* uninstallData);

#ifdef __cplusplus
}
#endif