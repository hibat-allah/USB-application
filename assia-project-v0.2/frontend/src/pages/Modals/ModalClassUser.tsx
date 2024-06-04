import Modal from "../../components/UI/Modal";
import { useState } from "react";
import axios from "axios";
import { baseURL } from "../../config";

type Props = {
  isOpen: boolean,
  close: () => void,
}

const types = ['HidUsb', 'WINUSB', 'RtlWlanu', 'USBSTOR', 'webcam', 'usbprint'];

export default function ModalExample({ isOpen, close }: Props) {
    const [guid, setGuid] = useState("");
    const [path, setPath] = useState("");
    const [type, setType] = useState(types[0]); // Default to the first type

    const submit = async (e: React.FormEvent) => {
        e.preventDefault();
        try {
            await axios.post(`${baseURL}/class`, {
                guid: guid,
                path: path,
                type: type
            });
            console.log("Class added successfully");
            close();
        } catch (error) {
            console.error("There was an error adding the class!", error);
        }
    }

    return (
        <Modal isOpen={isOpen} size="sm:max-w-2xl">
            <h3 className="text-lg font-semibold leading-6 text-gray-900 mb-3"> Ajouter une classe </h3>
            <p className="text-gray-600"> Remplissez ce formulaire pour ajouter une nouvelle classe.</p>
            <form onSubmit={submit}>
                <div className="grid grid-cols-6 gap-2">
                    <div className="col-span-6 sm:col-span-3">
                        <label htmlFor="guid" className="block text-sm font-medium text-gray-700">GUID</label>
                        <input
                            type="text"
                            name="guid"
                            id="guid"
                            className="mt-1 block w-full primary"
                            value={guid}
                            onChange={(e) => setGuid(e.target.value)}
                            required
                        />
                    </div>
                    <div className="col-span-6 sm:col-span-3">
                        <label htmlFor="path" className="block text-sm font-medium text-gray-700">Path</label>
                        <input
                            type="text"
                            name="path"
                            id="path"
                            className="mt-1 block w-full primary"
                            value={path}
                            onChange={(e) => setPath(e.target.value)}
                            required
                        />
                    </div>
                    <div className="col-span-6 sm:col-span-3">
                        <label htmlFor="type" className="block text-sm font-medium text-gray-700">Type</label>
                        <select
                            name="type"
                            id="type"
                            className="mt-1 block w-full primary"
                            value={type}
                            onChange={(e) => setType(e.target.value)}
                            required
                        >
                            {types.map((typeOption) => (
                                <option key={typeOption} value={typeOption}>
                                    {typeOption}
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
