import { ICarsApiResponse } from "src/interfaces/ICarsApiResponse.interface";
import { api } from "src/providers/api";
import { type ICreateCarFormData } from "src/validations/car.validation";
import { ICar } from "src/interfaces/ICar.interface";

interface IOptions {
  name?: string;
  order?: string;
  page?: number;
  pageSize?: number;
  brandName?: string;
}

export const getAllCars = async ({
  name = "",
  order = "desc",
  brandName = "",
  page = 1,
  pageSize = 9,
}: IOptions = {}) => {
  let params: IOptions = { order, page, pageSize };

  if (name !== "") params = { ...params, name };
  if (brandName !== "") params = { ...params, brandName };

  try {
    const response: { data: ICarsApiResponse } = await api.get("/cars", {
      params,
    });

    return response.data;
  } catch (err) {
    console.log(err);

    throw err;
  }
};

export const createCar = async (car: ICreateCarFormData, token: string) => {
  const payload = {
    name: car.name,
    model: car.model,
    year: car.year,
    brandId: car.brand,
    price: car.price,
    imageUrl: car.imageUrl,
  };

  console.log({ payload });

  try {
    const response: { data: ICar } = await api.post("/cars", payload, {
      headers: {
        Authorization: `Bearer ${token}`,
      },
    });

    return response.data;
  } catch (err) {
    return false;
  }
};

export const deleteCar = async (carId: string, token: string) => {
  try {
    const response = await api.delete(`/cars/${carId}`, {
      headers: {
        Authorization: `Bearer ${token}`,
      },
    });

    return response;
  } catch (err) {
    return false;
  }
};

export const editCar = async (
  carId: string,
  car: ICreateCarFormData,
  token: string,
) => {
  const payload = {
    name: car.name,
    model: car.model,
    year: car.year,
    price: car.price,
    imageUrl: car.imageUrl,
  };

  try {
    const response: { data: ICar } = await api.put(`/cars/${carId}`, payload, {
      headers: {
        Authorization: `Bearer ${token}`,
      },
    });

    return response.data;
  } catch (err) {
    return false;
  }
};
