import * as yup from "yup";
import { yupResolver } from "@hookform/resolvers/yup";

const loginSchema = yup
  .object({
    email: yup
      .string()
      .email("Formato de email inválido")
      .required("O email é obrigatório"),
    password: yup.string().required("A senha é obrigatória"),
  })
  .required();

export const loginResolver = yupResolver(loginSchema);

export type ILoginFormData = yup.InferType<typeof loginSchema>;
