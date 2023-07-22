import { BsX } from "react-icons/bs";

interface IProps {
  name: string;
}

export const ActiveFilter = ({ name }: IProps) => {
  return (
    <span className="flex items-center justify-center border-blue-600 border text-blue-600 rounded-md pl-2 pr-[2px] py-[2px] w-fit">
      <span className="text-sm">{name}</span>
      <BsX className="ml-1 text-2xl" />
    </span>
  );
};
