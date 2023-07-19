import { NavLink, useNavigate } from "react-router-dom";
import { RiAdminLine } from "react-icons/ri";
import { useCookies } from "react-cookie";
import { BiLogOutCircle } from "react-icons/bi";
import { UserRoles } from "src/enums";

export const Navbar = () => {
  const navigate = useNavigate();

  const [cookies, , removeCookie] = useCookies([
    "token",
    "userFirstName",
    "role",
  ]);

  const onLogout = () => {
    removeCookie("token", { path: "/" });
    removeCookie("userFirstName", { path: "/" });
    removeCookie("role", { path: "/" });
    navigate("/");
  };

  const onRegisterClick = () => {
    navigate("/register-car", {
      state: { editMode: false, car: null },
    });
  };

  const renderLoggedInContent = () => {
    return (
      <div className="flex items-center justify-between gap-8">
        <p className="mr-4">Olá, {cookies.userFirstName}.</p>

        {cookies.role === UserRoles.Admin && (
          <button
            onClick={onRegisterClick}
            className="hover:text-black transition-all flex items-center gap-2 text-lg leading-relaxed"
          >
            Adicionar Carro
          </button>
        )}

        <button
          onClick={onLogout}
          className="hover:text-black transition-all flex items-center gap-2 text-lg leading-relaxed"
        >
          <BiLogOutCircle />
          Logout
        </button>
      </div>
    );
  };

  const renderLoggedOutContent = () => {
    return (
      <NavLink to={"/login"}>
        <button className="hover:text-black transition-all flex items-center gap-2 text-lg">
          <RiAdminLine />
          Login
        </button>
      </NavLink>
    );
  };

  return (
    <nav className="flex justify-center h-20 bg-slate-100 drop-shadow-md px-4">
      <div className="w-full max-w-7xl flex justify-between items-center text-gray-700">
        <NavLink to={"/"}>
          <h1 className="hover:text-black transition-all text-lg">Carshop</h1>
        </NavLink>

        {cookies.token !== undefined
          ? renderLoggedInContent()
          : renderLoggedOutContent()}
      </div>
    </nav>
  );
};
