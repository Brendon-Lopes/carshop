import * as yup from "yup";
import { yupResolver } from "@hookform/resolvers/yup";

const createCarSchema = yup.object({
  name: yup
    .string()
    .required("O nome/descrição é obrigatório(a)")
    .min(2, "O nome/descrição deve ter no mínimo 2 caracteres"),
  brand: yup
    .string()
    .required("A marca é obrigatória")
    .min(2, "A marca deve ter no mínimo 2 caracteres"),
  model: yup.string().required("O modelo é obrigatório"),
  year: yup
    .number()
    .min(1900, "O ano deve ser maior ou igual a 1900")
    .max(2100, "Ano inválido")
    .typeError("O ano deve ser um número")
    .required("O ano é obrigatório")
    .test("len", "O ano deve ter 4 dígitos", (val) => {
      if (val !== undefined) return val.toString().length === 4;
      return false;
    })
    .test("max", "O ano deve ser menor ou igual ao ano atual", (val) => {
      if (val !== undefined) return val <= new Date().getFullYear();
      return false;
    }),
  price: yup
    .number()
    .typeError("O preço deve ser um número")
    .required("O preço é obrigatório")
    .test("min", "O preço deve ser maior que 0", (val) => {
      if (val !== undefined) return val > 0;
      return false;
    }),
  imageUrl: yup.string().url("URL inválida").required("A imagem é obrigatória"),
});

export const createCarResolver = yupResolver(createCarSchema);

export type ICreateCarFormData = yup.InferType<typeof createCarSchema>;
