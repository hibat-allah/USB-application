import Modal from "../../components/UI/Modal";
import { useState, useEffect } from "react";
import axios from "axios";
import { baseURL } from "../../config";

type Props = {
  isOpen: boolean,
  close: () => void,
};

export default function ModalDevices({ isOpen, close }: Props) {
  const [classes, setClasses] = useState<any[]>([]);
  useEffect(() => {
    axios.get(`${baseURL}/class`).then((response) => {
      setClasses(response.data);
    });
  }, []);

  const [users, setUsers] = useState<any[]>([]);
  useEffect(() => {
    axios.get(`${baseURL}/users`).then((response) => {
      setUsers(response.data);
    });
  }, []);
  const [selectedUsers, setSelectedUsers] = useState<string[]>([]);

  const handleUsersChange = (e: React.ChangeEvent<HTMLSelectElement>) => {
    const options = e.target.options;
    const selectedValues = Array.from(options)
      .filter(option => option.selected)
      .map(option => option.value);
    setSelectedUsers(selectedValues);
  };

  const [classe, setClasse] = useState("");
  const [device_id, setDeviceID] = useState("");
  const [nameD, setNameD] = useState("");

  const submit = async (e: React.FormEvent) => {
    e.preventDefault();
    try {
      await axios.post(`${baseURL}/peripherique`, { 
        idDevice: device_id,
        nameD: nameD,
        classI_id: classe,
      });
      console.log("Device added successfully");

      for (const user_id of selectedUsers) {
        await axios.post(`${baseURL}/DeviceUsers`, {
          user_id: user_id,
          device_id: device_id,
        });
        console.log("DeviceUser added successfully");
      }
      close();
    } catch (error) {
      console.error("There was an error adding the device!", error);
    }
  };

  return (
    <Modal isOpen={isOpen} size="sm:max-w-2xl">
      <h3 className="text-lg font-semibold leading-6 text-gray-900 mb-3">Ajouter un périphérique</h3>
      <p className="text-gray-600">Remplissez ce formulaire pour ajouter un nouveau périphérique.</p>
      <form onSubmit={submit}>
        <div className="grid grid-cols-6 gap-2">
          <div className="col-span-6 sm:col-span-3">
            <label htmlFor="device_id" className="block text-sm font-medium text-gray-700">ID Périphérique</label>
            <input
              type="text"
              name="device_id"
              id="device_id"
              className="mt-1 block w-full primary"
              value={device_id}
              onChange={(e) => setDeviceID(e.target.value)}
              required
            />
          </div>
          <div className="col-span-6 sm:col-span-3">
            <label htmlFor="nameD" className="block text-sm font-medium text-gray-700">Nom du Périphérique</label>
            <input
              type="text"
              name="nameD"
              id="nameD"
              className="mt-1 block w-full primary"
              value={nameD}
              onChange={(e) => setNameD(e.target.value)}
              required
            />
          </div>
          <div className="col-span-6 sm:col-span-3">
            <label htmlFor="classe" className="block text-sm font-medium text-gray-700">Classe</label>
            <select
              name="classe"
              id="classe"
              className="mt-1 block w-full primary"
              value={classe}
              onChange={(e) => setClasse(e.target.value)}
              required
            >
              <option value="">Sélectionner une classe</option>
              {classes.map((classOption) => (
                <option key={classOption.guid} value={classOption.guid}>
                  {classOption.type}
                </option>
              ))}
            </select>
          </div>
          <div className="col-span-6 sm:col-span-3">
            <label htmlFor="user_id" className="block text-sm font-medium text-gray-700">Utilisateur</label>
            <select
              name="user_id"
              id="user_id"
              className="mt-1 block w-full primary"
              multiple
              value={selectedUsers}
              onChange={handleUsersChange}
              required
            >
              <option value="">Sélectionner un utilisateur</option>
              {users.map((user) => (
                <option key={user.email} value={user.email}>
                  {user.email}
                </option>
              ))}
            </select>
          </div>
        </div>
        <div className="flex justify-end gap-3 mt-4">
          <button type="submit" className="bg-violet-500 rounded-md px-4 py-2 font-semibold text-white">Submit</button>
          <button type="button" className="bg-white px-3 font-semibold text-gray-900 ring-gray-300 hover:bg-gray-50" onClick={close}>Annuler</button>
        </div>
      </form>
    </Modal>
  );
}
