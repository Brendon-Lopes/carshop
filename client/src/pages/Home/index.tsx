import { useState, useEffect } from "react";
import { Navbar, SearchBar, CarCard, SideBar } from "src/components";
import { type IBrand, type ICar } from "src/interfaces";
import * as carsService from "src/services/car.service";
import * as brandsService from "src/services/brand.service";
import { BsChevronRight, BsChevronLeft } from "react-icons/bs";
import { useCookies } from "react-cookie";
import { UserRoles } from "src/enums";

export const Home = () => {
  const [cars, setCars] = useState<ICar[]>([]);
  const [brands, setBrands] = useState<IBrand[]>([]);
  const [currentPage, setCurrentPage] = useState<number>(1);
  const [totalPages, setTotalPages] = useState<number>(1);
  const [selectedBrand, setSelectedBrand] = useState("");

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
      alert("Você não tem permissão para deletar um carro");
      return;
    }

    void carsService
      .deleteCar(carId, token)
      .then((res) => {
        if (res === false) {
          alert("Não foi possível deletar o carro");
        }
      })
      .then(() => {
        alert("Carro deletado com sucesso");

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

        <section>
          <section className="grid grid-cols-2 md:grid-cols-3 mb-8">
            {cars.length > 0 &&
              cars.map((car) => (
                <CarCard key={car.id} car={car} onCarDelete={onCarDelete} />
              ))}
          </section>

          <div className="w-full flex mb-12">
            <section className="m-auto flex items-center text-lg">
              <button
                onClick={goPreviousPage}
                className="p-3 bg-gray-800 rounded-md mx-3"
              >
                <BsChevronLeft className="text-white" />
              </button>

              <p>{`${currentPage} de ${totalPages}`}</p>

              <button
                onClick={goNextPage}
                className="p-3 bg-gray-800 rounded-md mx-3"
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