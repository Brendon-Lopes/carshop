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

      <section className="mt-6 grid grid-cols-2 gap-4">
        <button className="success-btn" type="button" onClick={onConfirmation}>
          {confirmText ?? "Confirmar"}
        </button>

        <button className="danger-btn" type="button" onClick={onCancel}>
          {cancelText ?? "Cancelar"}
        </button>
      </section>
    </Modal>
  );
};
