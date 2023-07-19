import { Navbar, RegisterCarForm } from "src/components";
import { useCookies } from "react-cookie";

export function RegisterCar() {
  const [cookies] = useCookies(["token"]);

  if (cookies.token === undefined) {
    return (
      <div>
        <Navbar />
        <section className="flex justify-center max-w-7xl m-auto mt-6 p-4">
          <p className="text-2xl">
            403 Forbidden - Você não tem permissão para acessar essa página.
          </p>
        </section>
      </div>
    );
  }

  return (
    <div>
      <Navbar />
      <section className="flex max-w-7xl m-auto mt-6 p-4">
        <RegisterCarForm />
      </section>
    </div>
  );
}
