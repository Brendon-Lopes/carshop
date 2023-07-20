import { HttpStatusCode } from "axios";
import { api } from "../providers/api";
import { IBrand } from "src/interfaces";

export const getAllBrands = async () => {
  try {
    const response: { data: IBrand } = await api.get("/brands");

    return response.data;
  } catch (err) {
    console.log(err);
  }
};

export const createBrand = async (name: string, token: string) => {
  try {
    const response: { data: IBrand } = await api.post(
      "/brands",
      {
        name,
      },
      {
        headers: {
          Authorization: `Bearer ${token}`,
        },
      }
    );

    return response.data;
  } catch (err: any) {
    console.log(err);

    if (err?.response?.status === HttpStatusCode.Conflict) {
      return HttpStatusCode.Conflict;
    }

    throw err;
  }
};
