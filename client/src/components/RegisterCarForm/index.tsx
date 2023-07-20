import { useEffect, useState } from "react";
import { useForm } from "react-hook-form";
import { ICar, type IBrand } from "src/interfaces";
import * as brandService from "src/services/brand.service";
import * as carService from "src/services/car.service";
import { useCookies } from "react-cookie";
import { useNavigate, useLocation } from "react-router-dom";
import { ConfirmationModal, Modal } from "src/components";
import {
  createCarResolver,
  type ICreateCarFormData,
} from "src/validations/car.validation";
import { HttpStatusCode } from "axios";

export const RegisterCarForm = () => {
  const location = useLocation();
  const state = location.state as { editMode: boolean; car: ICar };
  const editMode = state.editMode;

  const [brands, setBrands] = useState<IBrand[]>([]);
  const [isModalOpen, setIsModalOpen] = useState<boolean>(false);
  const [newBrandName, setNewBrandName] = useState<string>("");
  const [newBrandLengthError, setNewBrandLengthError] =
    useState<boolean>(false);
  const [isConfirmationModalOpen, setIsConfirmationModalOpen] =
    useState<boolean>(false);

  const [selectedBrand, setSelectedBrand] = useState<string>(
    editMode ? state.car.brandId : ""
  );

  const [triggerReloadBrands, setTriggerReloadBrands] =
    useState<boolean>(false);

  const [cookies] = useCookies(["token"]);

  const navigate = useNavigate();

  const formMethods = useForm<ICreateCarFormData>({
    resolver: createCarResolver,
  });

  const {
    formState: { errors },
    register,
    handleSubmit,
    setValue,
  } = formMethods;

  const onSubmit = (data: ICreateCarFormData) => {
    const token = cookies.token as string;

    void carService.createCar(data, token).then((res) => {
      if (res === false) {
        alert("Erro ao cadastrar!");
        return;
      }

      alert("Carro cadastrado com sucesso!");
      navigate("/");
    });
  };

  const onEdit = (data: ICreateCarFormData) => {
    const token = cookies.token as string;

    void carService.editCar(state.car.id, data, token).then((res) => {
      if (res === false) {
        alert("Erro ao editar!");
        return;
      }

      alert("Carro editado com sucesso!");
      navigate("/");
    });
  };

  const handleCreateBrand = (event: React.FormEvent<HTMLFormElement>) => {
    event.preventDefault();

    if (newBrandName.length < 2) {
      setNewBrandLengthError(true);
      return;
    }

    void brandService
      .createBrand(newBrandName, cookies.token as string)
      .then((res) => {
        if (res === HttpStatusCode.Conflict) {
          alert("Já existe uma marca com esse nome!");
          return;
        }

        alert("Marca criada com sucesso!");
        setIsModalOpen(false);
        setNewBrandName("");
        setNewBrandLengthError(false);
        setTriggerReloadBrands((prev) => !prev);
      });
  };

  const handleCancel = () => {
    setIsConfirmationModalOpen(true);
  };

  useEffect(() => {
    void brandService.getAllBrands().then((res) => {
      setBrands(res as unknown as IBrand[]);
    });

    setValue("name", editMode ? state.car.name : "");
    setValue("model", editMode ? state.car.model : "");
    setValue("year", editMode ? state.car.year : 0);
    setValue("price", editMode ? state.car.price : 0);
    setValue("imageUrl", editMode ? state.car.imageUrl : "");
    setValue("brand", editMode ? state.car.brandId : "");
  }, [editMode]);

  useEffect(() => {
    void brandService.getAllBrands().then((res) => {
      setBrands(res as unknown as IBrand[]);
    });
  }, [triggerReloadBrands]);

  return (
    <>
      <form
        onSubmit={handleSubmit(editMode ? onEdit : onSubmit)}
        className="w-full sm:w-full md:w-3/4 lg:w-1/2 m-auto mt-6"
      >
        <div className="mb-6">
          <label
            htmlFor="name"
            className="block mb-2 text-sm font-medium text-gray-900"
          >
            Nome / Descrição
          </label>
          <input
            type="text"
            {...register("name")}
            className="bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg focus:ring-blue-500 focus:border-blue-500 block w-full p-2.5"
          />
          {errors.name !== undefined && (
            <p className="error-message">{errors.name.message}</p>
          )}
        </div>

        <div className="mb-6">
          <label
            htmlFor="model"
            className="block mb-2 text-sm font-medium text-gray-900"
          >
            Modelo
          </label>
          <input
            type="text"
            {...register("model")}
            className="bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg focus:ring-blue-500 focus:border-blue-500 block w-full p-2.5"
          />
          {errors.model !== undefined && (
            <p className="error-message">{errors.model.message}</p>
          )}
        </div>

        <div className="mb-6">
          <label
            htmlFor="brand"
            className="block mb-2 text-sm font-medium text-gray-900"
          >
            Marca
          </label>
          <section className="grid grid-cols-3 gap-4">
            <select
              {...register("brand")}
              value={selectedBrand}
              onChange={(e) => setSelectedBrand(e.target.value)}
              className="col-span-2 bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg focus:ring-blue-500 focus:border-blue-500 w-full p-2.5"
            >
              <option disabled value="">
                Selecione uma marca
              </option>
              {brands.map((brand) => (
                <option key={brand.id} value={brand.id}>
                  {brand.name}
                </option>
              ))}
            </select>

            <button
              type="button"
              className="col-span-1 text-white bg-green-700 hover:bg-green-800 focus:ring-4 focus:outline-none focus:ring-green-400 font-medium rounded-lg text-sm sm:w-auto px-5 py-2.5 text-center"
              onClick={() => setIsModalOpen(true)}
            >
              Cadastrar nova marca
            </button>
          </section>
          {errors.brand !== undefined && (
            <p className="error-message">{errors.brand.message}</p>
          )}
        </div>

        <div className="mb-6">
          <label
            htmlFor="year"
            className="block mb-2 text-sm font-medium text-gray-900"
          >
            Ano
          </label>
          <input
            type="number"
            {...register("year")}
            className="bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg focus:ring-blue-500 focus:border-blue-500 block w-full p-2.5"
          />
          {errors.year !== undefined && (
            <p className="error-message">{errors.year.message}</p>
          )}
        </div>

        <div className="mb-6">
          <label
            htmlFor="price"
            className="block mb-2 text-sm font-medium text-gray-900"
          >
            Preço
          </label>
          <input
            type="number"
            {...register("price")}
            className="bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg focus:ring-blue-500 focus:border-blue-500 block w-full p-2.5"
          />
          {errors.price !== undefined && (
            <p className="error-message">{errors.price.message}</p>
          )}
        </div>

        <div className="mb-6">
          <label
            htmlFor="imageUrl"
            className="block mb-2 text-sm font-medium text-gray-900"
          >
            URL da imagem
          </label>
          <input
            type="url"
            {...register("imageUrl")}
            className="bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg focus:ring-blue-500 focus:border-blue-500 block w-full p-2.5"
          />
          {errors.imageUrl !== undefined && (
            <p className="error-message">{errors.imageUrl.message}</p>
          )}
        </div>

        <button
          type="submit"
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
            w-full
            sm:w-auto
            px-5
            py-2.5
            text-center
            mr-2
          "
        >
          {editMode ? "Editar" : "Cadastrar"}
        </button>

        <button
          type="button"
          className="
            text-white
            bg-red-700
            hover:bg-blredue-800
            focus:ring-4
            focus:outline-none
            focus:ring-red-300
            font-medium
            rounded-lg
            text-sm w-full
            sm:w-auto px-5
            py-2.5
            text-center
          "
          onClick={handleCancel}
        >
          Cancelar
        </button>
      </form>

      <ConfirmationModal
        isOpen={isConfirmationModalOpen}
        onConfirmation={() => {
          setIsConfirmationModalOpen(false);
          navigate("/");
        }}
        onCancel={() => setIsConfirmationModalOpen(false)}
        cancelText={`Voltar para ${editMode ? "a edição" : "o cadastro"}`}
      >
        <h1 className="block mb-2 font-medium text-gray-900 text-center">
          {`Tem certeza que deseja cancelar ${
            editMode ? "a edição" : "o cadastro"
          }?`}
        </h1>
      </ConfirmationModal>

      <Modal isOpen={isModalOpen}>
        <form onSubmit={handleCreateBrand}>
          <h2 className="block mb-2 text-sm font-medium text-gray-900">
            Criar nova marca
          </h2>

          <input
            className="
            bg-gray-50
            border
            border-gray-300
            text-gray-900
            text-sm rounded-lg
            focus:ring-blue-500
            focus:border-blue-500
            block
            w-full
            p-2.5
            mb-2
          "
            placeholder="Nome da marca"
            type="text"
            value={newBrandName}
            onChange={(e) => setNewBrandName(e.target.value)}
          />

          {newBrandLengthError && (
            <p className="error-message mb-4">
              O nome da marca deve ter no mínimo 2 caracteres
            </p>
          )}

          <section className="mt-6 flex gap-4">
            <button
              type="submit"
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
            >
              Criar marca
            </button>

            <button
              className="
                text-white
                bg-red-700
                hover:bg-blredue-800
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
              onClick={() => {
                setIsModalOpen(false);
                setNewBrandName("");
                setNewBrandLengthError(false);
              }}
            >
              Cancelar
            </button>
          </section>
        </form>
      </Modal>
    </>
  );
};
