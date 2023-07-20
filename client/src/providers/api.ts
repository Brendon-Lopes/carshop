import axios, { AxiosInstance } from "axios";

const baseURL = import.meta.env.VITE_API_URL as string;

export const api: AxiosInstance = axios.create({ baseURL });
