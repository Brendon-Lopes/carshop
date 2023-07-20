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
        </form>

        <BsSearch className="absolute right-2 text-xl text-gray-600" />
      </div>
    </div>
  );
};
