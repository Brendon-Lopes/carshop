import { type ICar } from "./ICar.interface";

export interface ICarsApiResponse {
  cars: ICar[];
  currentPage: number;
  totalPages: number;
}
