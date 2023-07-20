import { type ICar } from "src/interfaces";
import { formatPrice } from "src/utils";
import { useCookies } from "react-cookie";
import { BsFillTrashFill, BsFillPencilFill } from "react-icons/bs";
import { useNavigate } from "react-router-dom";
import { UserRoles } from "src/enums";
import { ConfirmationModal } from "src/components";
import { useState } from "react";

interface IProps {
  car: ICar;
  onCarDelete: (carId: string) => void;
}

export const CarCard = ({ car, onCarDelete }: IProps) => {
  const [cookies] = useCookies(["token", "role"]);

  const navigate = useNavigate();

  const [isConfirmationModalOpen, setIsConfirmationModalOpen] =
    useState<boolean>(false);

  const onCarEdit = () => {
    navigate("/edit-car", { state: { car, editMode: true } });
  };

  return (
    <>
      <div className="w-full p-4 relative">
        {cookies.role === UserRoles.Admin && (
          <>
            <BsFillPencilFill
              onClick={onCarEdit}
              data-testid="edit-button"
              className="absolute text-2xl text-gray-900 bottom-9 right-20 hover:cursor-pointer hover:scale-125 transition-all"
            />
            <BsFillTrashFill
              data-testid="delete-button"
              className="absolute text-2xl text-red-600 bottom-9 right-9 hover:cursor-pointer hover:scale-125 transition-all"
              onClick={() => setIsConfirmationModalOpen(true)}
            />
          </>
        )}

        <div className="h-full bg-white rounded-lg shadow-md hover:shadow-lg hover:cursor-pointer">
          <img
            className="w-full object-cover h-48 rounded-t-lg"
            src={car.imageUrl}
            alt={car.name}
          />
          <div className="p-4">
            <div className="font-bold text-xl mb-2">{car.name}</div>
            <p className="text-gray-700 text-base">{`${car.brandName} - ${car.model} - Ano ${car.year}`}</p>
            <div className="mt-4">
              <span className="inline-block text-2xl text-blue-600 mr-2 mb-2">
                {formatPrice(car.price)}
              </span>
            </div>
          </div>
        </div>
      </div>

      <ConfirmationModal
        isOpen={isConfirmationModalOpen}
        onConfirmation={() => onCarDelete(car.id)}
        onCancel={() => setIsConfirmationModalOpen(false)}
      >
        <h1 className="block mb-2 font-medium text-gray-900">
          Tem certeza que deseja excluir o carro{" "}
          <span className="text-red-600">{car.name}</span>? Essa ação não poderá
          ser desfeita.
        </h1>
      </ConfirmationModal>
    </>
  );
};
