import { IoFilter } from "react-icons/io5";
import { type IBrand } from "src/interfaces";

interface IProps {
  brands: IBrand[];
  selectedBrand: string;
  setSelectedBrand: (brandId: string) => void;
  setSearchValue: (search: string) => void;
  setRefreshTrigger: (callback: (prevState: boolean) => boolean) => void;
}

export const SideBar = ({
  brands,
  selectedBrand,
  setSelectedBrand,
  setSearchValue,
  setRefreshTrigger,
}: IProps) => {
  const handleBrandSelection = (event: React.ChangeEvent<HTMLInputElement>) => {
    setSelectedBrand(event.target.id);
  };

  return (
    <aside className="w-3/12 transition-transform" aria-label="Sidebar">
      <div className="h-full px-4 py-4 overflow-y-auto">
        <button
          onClick={() => {
            setSelectedBrand("");
            setSearchValue("");
            setRefreshTrigger((prevState) => !prevState);
          }}
          className="text-gray-500 mb-4"
        >
          Resetar filtros
        </button>

        <span className="flex items-center text-base text-gray-900">
          <IoFilter />
          <span className="ml-3">Filtrar por marca</span>
        </span>

        <ul className="mt-2">
          {brands.map((brand) => (
            <li key={brand.id} className="mb-1">
              <input
                type="radio"
                id={brand.name}
                checked={brand.name === selectedBrand}
                onChange={handleBrandSelection}
                className="mr-2"
              />
              <label htmlFor={brand.name}>{brand.name}</label>
            </li>
          ))}
        </ul>
      </div>
    </aside>
  );
};
