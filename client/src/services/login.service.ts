import { api } from "../providers/api";

export const login = async (email: string, password: string) => {
  try {
    const response = await api.post("/auth/login", {
      email,
      password,
    });

    return response.data;
  } catch (error: any) {
    console.log(error);
    if (error.response && error.response.status === 400) {
      return false;
    }
    throw error;
  }
};
