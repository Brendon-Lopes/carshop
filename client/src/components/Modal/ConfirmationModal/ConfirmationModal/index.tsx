import { Modal } from "src/components";

interface IProps {
  children: React.ReactNode;
  isOpen: boolean;
  onConfirmation: () => void;
  onCancel: () => void;
  confirmText?: string;
  cancelText?: string;
}

export const ConfirmationModal = ({
  children,
  isOpen,
  onConfirmation,
  onCancel,
  confirmText,
  cancelText,
}: IProps) => {
  return (
    <Modal isOpen={isOpen}>
      {children}

      <section className="mt-6 flex gap-4">
        <button
          className="
          text-white
          bg-blue-700
          hover:bg-blue-800
          focus:ring-4
          focus:outline-none
          focus:ring-blue-300
          font-medium
          rounded-lg
          text-sm
          px-5
          py-2.5
          text-center
          flex-grow
        "
          type="button"
          onClick={onConfirmation}
        >
          {confirmText ?? "Confirmar"}
        </button>

        <button
          className="
          text-white
          bg-red-700
          hover:bg-red-800
          focus:ring-4
          focus:outline-none
          focus:ring-red-300
          font-medium
          rounded-lg
          text-sm
          px-5
          py-2.5
          text-center
          flex-grow
        "
          type="button"
          onClick={onCancel}
        >
          {cancelText ?? "Cancelar"}
        </button>
      </section>
    </Modal>
  );
};
