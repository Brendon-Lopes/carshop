import { ILoginResponse } from "src/interfaces";
import { api } from "../providers/api";

export const login = async (email: string, password: string) => {
  try {
    const response: { data: ILoginResponse } = await api.post("/auth/login", {
      email,
      password,
    });

    return response.data;
  } catch (error: any) {
    console.log(error);
    if (error.response && error.response.status === 401) {
      return false;
    }
    throw error;
  }
};
