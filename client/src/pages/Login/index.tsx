import { Navbar } from "@components/Navbar";
import { useForm } from "react-hook-form";
import {
  type ILoginFormData,
  loginResolver,
} from "@validations/login.validation";
import * as loginService from "@services/login.service";
import { useState } from "react";
import { useCookies } from "react-cookie";
import { useNavigate } from "react-router-dom";

export function Login() {
  const navigate = useNavigate();

  const [invalidCredentials, setInvalidCredentials] = useState(false);

  const [, setCookies] = useCookies(["token", "userFirstName", "role"]);

  const formMethods = useForm<ILoginFormData>({ resolver: loginResolver });

  const {
    formState: { errors },
    register,
    handleSubmit,
  } = formMethods;

  const onLogin = (data: ILoginFormData) => {
    void loginService.login(data.email, data.password).then((res) => {
      if (res === false) {
        setInvalidCredentials(true);
        return;
      }

      setInvalidCredentials(false);
      setCookies("token", res.token, { path: "/" });
      setCookies("userFirstName", res.firstName, { path: "/" });
      setCookies("role", res.role, { path: "/" });
      navigate("/");
    });
  };

  return (
    <div className="h-screen">
      <Navbar />

      <section className="h-full mt-12">
        <div className="flex flex-col items-center justify-center px-6 py-8 mx-auto lg:py-0">
          <div className="w-full bg-white rounded-lg shadow md:mt-0 sm:max-w-md xl:p-0">
            <div className="p-6 space-y-4 md:space-y-6 sm:p-8">
              <h1 className="text-xl font-bold leading-tight tracking-tight text-gray-900 md:text-2xl">
                Faça o login na sua conta
              </h1>

              <form
                onSubmit={handleSubmit(onLogin)}
                className="space-y-4 md:space-y-6"
              >
                <div>
                  <label
                    htmlFor="email"
                    className="block mb-2 text-sm font-medium text-gray-900 dark:text-white"
                  >
                    Email
                  </label>
                  <input
                    {...register("email")}
                    type="email"
                    className="bg-gray-50 border border-gray-300 text-gray-900 sm:text-sm rounded-lg focus:ring-primary-600 focus:border-primary-600 block w-full p-2.5"
                    placeholder="email@exemplo.com"
                  />
                  {errors.email != null && (
                    <p className="error-message">{errors.email.message}</p>
                  )}
                </div>

                <div>
                  <label
                    htmlFor="password"
                    className="block mb-2 text-sm font-medium text-gray-900"
                  >
                    Senha
                  </label>

                  <input
                    {...register("password")}
                    type="password"
                    placeholder="••••••••"
                    className="bg-gray-50 border border-gray-300 text-gray-900 sm:text-sm rounded-lg focus:ring-primary-600 focus:border-primary-600 block w-full p-2.5"
                  />
                  {errors.password != null && (
                    <p className="error-message">{errors.password.message}</p>
                  )}
                </div>

                <button
                  type="submit"
                  className="w-full text-white bg-blue-600 hover:bg-blue-700 focus:ring-4 focus:outline-none focus:ring-blue-300 font-medium rounded-lg text-sm px-5 py-2.5 text-center"
                >
                  Login
                </button>

                {invalidCredentials && (
                  <p className="error-message text-center">
                    Email e/ou senha inválidos
                  </p>
                )}
              </form>
            </div>
          </div>
        </div>
      </section>
    </div>
  );
}
