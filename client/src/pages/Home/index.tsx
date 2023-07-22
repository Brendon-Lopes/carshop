import { useState, useEffect } from "react";
import { Navbar, SearchBar, CarCard, SideBar } from "src/components";
import { type IBrand, type ICar } from "src/interfaces";
import * as carsService from "src/services/car.service";
import * as brandsService from "src/services/brand.service";
import { BsChevronRight, BsChevronLeft } from "react-icons/bs";
import { useCookies } from "react-cookie";
import { UserRoles } from "src/enums";
import { toast } from "react-toastify";
import { CarCardSkeleton } from "src/components";

export const Home = () => {
  const [cars, setCars] = useState<ICar[]>([]);
  const [brands, setBrands] = useState<IBrand[]>([]);
  const [currentPage, setCurrentPage] = useState<number>(1);
  const [totalPages, setTotalPages] = useState<number>(1);
  const [selectedBrand, setSelectedBrand] = useState("");
  const [isLoading, setIsLoading] = useState<boolean>(true);

  const [cookies] = useCookies(["token", "role"]);

  const onSearch = (search: string) => {
    void carsService.getAllCars({ name: search }).then((response) => {
      if (response) setCars(response.cars);
    });
  };

  const goPreviousPage = () => {
    if (currentPage > 1) {
      void carsService
        .getAllCars({ page: currentPage - 1 })
        .then((response) => {
          setCars(response.cars);
          setCurrentPage(response.currentPage);
          setTotalPages(response.totalPages);
        });

      window.scrollTo(0, 0);
    }
  };

  const goNextPage = () => {
    if (currentPage < totalPages) {
      void carsService
        .getAllCars({ page: currentPage + 1 })
        .then((response) => {
          setCars(response.cars);
          setCurrentPage(response.currentPage);
          setTotalPages(response.totalPages);
        });

      window.scrollTo(0, 0);
    }
  };

  const onCarDelete = (carId: string) => {
    const token = cookies.token as string;

    if (cookies.role !== UserRoles.Admin) {
      toast.error("Você não tem permissão para deletar um carro");

      return;
    }

    void carsService
      .deleteCar(carId, token)
      .then((res) => {
        if (res === false) {
          toast.error("Não foi possível deletar o carro");
        }
      })
      .then(() => {
        toast.success("Carro deletado com sucesso!");

        void carsService.getAllCars().then((response) => {
          setCars(response.cars);
          setCurrentPage(response.currentPage);
          setTotalPages(response.totalPages);
        });
      });
  };

  useEffect(() => {
    void carsService.getAllCars().then((response) => {
      setCars(response.cars);
      setCurrentPage(response.currentPage);
      setTotalPages(response.totalPages);
      setIsLoading(false);
    });

    void brandsService.getAllBrands().then((response) => {
      setBrands(response as unknown as IBrand[]);
    });
  }, []);

  useEffect(() => {
    if (selectedBrand !== "") {
      void carsService
        .getAllCars({ brandName: selectedBrand })
        .then((response) => {
          setCars(response.cars);
          setCurrentPage(response.currentPage);
          setTotalPages(response.totalPages);
        });
    } else {
      void carsService.getAllCars().then((response) => {
        setCars(response.cars);
        setCurrentPage(response.currentPage);
        setTotalPages(response.totalPages);
      });
    }
  }, [selectedBrand]);

  return (
    <div>
      <Navbar />
      <SearchBar onSearch={onSearch} />
      <main className="flex max-w-7xl m-auto mt-6">
        <SideBar
          brands={brands}
          selectedBrand={selectedBrand}
          setSelectedBrand={setSelectedBrand}
        />

        <section className="w-full">
          <section className="grid grid-cols-2 md:grid-cols-3 mb-8 min-h-[70vh]">
            {isLoading ? (
              <>
                {Array.from(Array(9).keys()).map((_, index) => (
                  <CarCardSkeleton key={index} />
                ))}
              </>
            ) : (
              cars.map((car) => (
                <CarCard key={car.id} car={car} onCarDelete={onCarDelete} />
              ))
            )}
            {cars.length === 0 && (
              <div className="flex items-center justify-center w-full h-full col-span-3">
                <p className="text-center text-xl text-gray-500">
                  Nenhum carro encontrado
                </p>
              </div>
            )}
          </section>

          <div className="w-full flex mb-12">
            <section className="m-auto flex items-center text-lg">
              <button
                onClick={goPreviousPage}
                className={`p-3 ${
                  currentPage == 1
                    ? "bg-gray-300 cursor-default"
                    : "bg-gray-800"
                } rounded-md mx-3`}
              >
                <BsChevronLeft className="text-white" />
              </button>

              <p>{`${currentPage} de ${totalPages}`}</p>

              <button
                onClick={goNextPage}
                className={`p-3 ${
                  currentPage == totalPages
                    ? "bg-gray-300 cursor-default"
                    : "bg-gray-800"
                } rounded-md mx-3`}
              >
                <BsChevronRight className="text-white" />
              </button>
            </section>
          </div>
        </section>
      </main>
    </div>
  );
};
