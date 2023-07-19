import { BrowserRouter, Route, Routes } from "react-router-dom";
import { Home } from "@pages/Home/";
import { Login } from "@pages/Login";

export const AppRouter = () => {
  return (
    <BrowserRouter>
      <Routes>
        <Route path="/" element={<Home />} />
        <Route path="/login" element={<Login />} />
        <Route path="/register-car" element={<Home />} />
        <Route path="/edit-car" element={<Home />} />
        <Route path="*" element={<h1>404 Page Not Found</h1>} />
      </Routes>
    </BrowserRouter>
  );
};
