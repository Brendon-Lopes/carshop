import { type ICar } from "src/interfaces";
import { formatPrice, formatText } from "src/utils";
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
      <div className="w-full max-h-[470px] lg:max-h-[420px] p-4 relative min-h-[390px]">
        <div className="h-full bg-white rounded-lg shadow-md hover:shadow-lg hover:cursor-pointer">
          <img
            className="w-full object-cover h-48 rounded-t-lg"
            src={car.imageUrl}
            alt={car.name}
          />
          <div className="p-4">
            <h1 className="font-bold text-xl mb-2">
              {formatText(car.name, 23)}
            </h1>
            <p className="text-gray-700 text-base">
              {formatText(`${car.brandName} - ${car.model} - Ano ${car.year}`)}
            </p>
            <div className="mt-4">
              <span className="inline-block text-2xl text-blue-600 mr-2 mb-2">
                {formatPrice(car.price)}
              </span>
            </div>
          </div>
          {cookies.role === UserRoles.Admin && (
            <div className="flex items-center justify-end mb-4">
              <BsFillPencilFill
                onClick={onCarEdit}
                data-testid="edit-button"
                className="text-2xl text-gray-900 hover:cursor-pointer hover:scale-125 transition-all mr-4"
              />
              <BsFillTrashFill
                data-testid="delete-button"
                className="text-2xl text-red-600 hover:cursor-pointer hover:scale-125 transition-all mr-4"
                onClick={() => setIsConfirmationModalOpen(true)}
              />
            </div>
          )}
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
