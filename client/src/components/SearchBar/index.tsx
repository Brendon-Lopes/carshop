import { useState } from "react";
import { BsSearch } from "react-icons/bs";

interface IProps {
  onSearch: (search: string) => void;
}

export const SearchBar = ({ onSearch }: IProps) => {
  const [searchValue, setSearchValue] = useState<string>("");

  const onInputChange = (event: React.ChangeEvent<HTMLInputElement>) => {
    setSearchValue(event.target.value);
  };

  const onFormSubmit = (event: React.FormEvent<HTMLFormElement>) => {
    event.preventDefault();
    onSearch(searchValue);
  };

  return (
    <div className="bg-blue-600 h-16 flex justify-center items-center px-4">
      <div className="relative max-w-7xl h-4/6 w-full flex items-center">
        <form onSubmit={onFormSubmit} className="w-full h-full">
          <input
            className="rounded-sm p-2 w-full h-full"
            type="text"
            placeholder="Buscar por marca ou modelo..."
            value={searchValue}
            onChange={onInputChange}
          />

          <button
            className="
              absolute
              right-0
              px-3 h-full
              text-gray-400
              hover:text-gray-800
              transition
              duration-200
            "
            data-testid="submit-btn"
            type="submit"
          >
            <BsSearch className="text-xl" />
          </button>
        </form>
      </div>
    </div>
  );
};
