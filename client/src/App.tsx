import { ToastContainer, toast } from "react-toastify";
import "react-toastify/dist/ReactToastify.css";
import { AppRouter } from "./router";

export default function App() {
  return (
    <>
      <AppRouter />
      <ToastContainer position={toast.POSITION.BOTTOM_RIGHT} theme="light" />
    </>
  );
}
