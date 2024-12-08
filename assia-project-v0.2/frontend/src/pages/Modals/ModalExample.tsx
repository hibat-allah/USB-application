import Modal from "../../components/UI/Modal";
import { useState } from "react";
import axios from "axios";
import { baseURL } from "../../config";

type Props = {
  isOpen: boolean,
  close: () => void,
}

export default function ModalExample({isOpen, close}: Props) {
    const [email, setEmail] = useState("");
    const [username, setUsername] = useState("");

    const submit = async (e: React.FormEvent) => {
        e.preventDefault();
        try {
            await axios.post(`${baseURL}/users`, { 
                email: email,
                username: username
            });
            console.log("User added successfully");
            close();
        } catch (error) {
            console.error("There was an error adding the user!", error);
        }
    }

    return (
        <Modal isOpen={isOpen} size="sm:max-w-2xl">
            <h3 className="text-lg font-semibold leading-6 text-gray-900 mb-3"> Ajouter un utilisateur </h3>
            <p className="text-gray-600"> Remplissez ce formulaire pour ajouter un nouvel utilisateur.</p>
            <form onSubmit={submit}>
                <div className="grid grid-cols-6 gap-2">
                    <div className="col-span-6 sm:col-span-3">
                        <label htmlFor="email" className="block text-sm font-medium text-gray-700">Email</label>
                        <input
                            type="email"
                            name="email"
                            id="email"
                            className="mt-1 block w-full primary"
                            value={email}
                            onChange={(e) => setEmail(e.target.value)}
                            required
                        />
                    </div>
                    <div className="col-span-6 sm:col-span-3">
                        <label htmlFor="username" className="block text-sm font-medium text-gray-700">Nom d'utilisateur</label>
                        <input
                            type="text"
                            name="username"
                            id="username"
                            className="mt-1 block w-full primary"
                            value={username}
                            onChange={(e) => setUsername(e.target.value)}
                            required
                        />
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
