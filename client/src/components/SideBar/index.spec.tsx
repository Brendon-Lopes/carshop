import "@testing-library/jest-dom";
import { render } from "@testing-library/react";
import { describe, expect, it, vi } from "vitest";
import { SideBar } from ".";
import { IBrand } from "src/interfaces";

describe("Sidebar component", () => {
  it("should render the brands and basic text", () => {
    const brandsMock: IBrand[] = [
      {
        id: "1",
        name: "Audi",
        createdAt: "2021-09-26T22:05:05.000Z",
        updatedAt: "2021-09-26T22:05:05.000Z",
      },
      {
        id: "2",
        name: "BMW",
        createdAt: "2021-09-26T22:05:05.000Z",
        updatedAt: "2021-09-26T22:05:05.000Z",
      },
    ];

    const sut = render(
      <SideBar
        brands={brandsMock}
        selectedBrand=""
        setSelectedBrand={vi.fn()}
        resetAllFilters={vi.fn()}
      />
    );

    expect(sut.getByText(/audi/i)).toBeInTheDocument();
    expect(sut.getByText(/bmw/i)).toBeInTheDocument();

    expect(sut.getByText(/filtrar por marca/i)).toBeInTheDocument();
    expect(sut.getByText(/resetar filtros/i)).toBeInTheDocument();
  });

  it("should render the selected brand", () => {
    const brandsMock: IBrand[] = [
      {
        id: "1",
        name: "Audi",
        createdAt: "2021-09-26T22:05:05.000Z",
        updatedAt: "2021-09-26T22:05:05.000Z",
      },
      {
        id: "2",
        name: "BMW",
        createdAt: "2021-09-26T22:05:05.000Z",
        updatedAt: "2021-09-26T22:05:05.000Z",
      },
    ];

    const sut = render(
      <SideBar
        brands={brandsMock}
        selectedBrand="Audi"
        setSelectedBrand={vi.fn()}
        resetAllFilters={vi.fn()}
      />
    );

    expect(sut.getByRole("radio", { name: "Audi" })).toBeChecked();
    expect(sut.getByRole("radio", { name: "BMW" })).not.toBeChecked();
  });
});
