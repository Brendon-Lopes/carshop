import { IBaseEntity } from "./IBaseEntity.interface";

export interface ICar extends IBaseEntity {
  name: string;
  model: string;
  year: number;
  price: number;
  imageUrl: string;
  brandId: string;
  brandName: string;
}
