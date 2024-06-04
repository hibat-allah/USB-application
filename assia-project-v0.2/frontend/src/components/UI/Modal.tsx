import { ReactNode } from "react";
import { Transition } from "@headlessui/react";

type ModalProps = {
  size: string;
  isOpen: boolean;
  children: ReactNode[];
};

function Modal({ size, isOpen, children }: ModalProps) {
  return (
    <Transition
      className="z-50 fixed"
      show={isOpen}
      enter="transition-opacity duration-75"
      enterFrom="opacity-0"
      enterTo="opacity-100"
      leave="transition-opacity duration-150"
      leaveFrom="opacity-100"
      leaveTo="opacity-0"
    >
      <div className="fixed inset-0 bg-black/50">
        <div className="flex min-h-full items-end justify-center p-4 text-center sm:items-center sm:p-0">
          <div className={`relative transform rounded-md bg-white text-left shadow-xl transition-all sm:my-8 sm:w-full ${size} max-h-[96vh] overflow-auto`} style={{ overflow: "auto" }}>
            <div className="px-4 pb-4 pt-5 sm:p-6">
              <div className="sm:flex sm:items-start w-full gap-4">
                <div className="mt-3 text-center sm:ml-2 sm:mt-0 sm:text-left w-full">
                  {children}
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>
    </Transition>
  );
}

export default Modal;
