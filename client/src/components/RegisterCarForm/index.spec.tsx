import "@testing-library/jest-dom";
import { render, screen } from "@testing-library/react";
import { afterAll, describe, expect, it, vi } from "vitest";
import { RegisterCarForm } from ".";
import { BrowserRouter } from "react-router-dom";
import * as router from "react-router-dom";
import { carMock } from "src/__tests__/mocks/carMock";
import * as brandServices from "src/services/brand.service";

vi.mock("react-router-dom", async () => {
  const actual: typeof router = await vi.importActual("react-router-dom");

  return {
    ...actual,
    useLocation: () => ({
      state: {
        editMode: false,
        car: null,
      },
    }),
  };
});

vi.mock("src/services/brand.service", async () => {
  const actual: typeof brandServices = await vi.importActual(
    "src/services/brand.service"
  );

  return {
    ...actual,
    getBrands: () => Promise.resolve([]),
  };
});

describe("RegisterCarForm component", () => {
  afterAll(() => {
    vi.restoreAllMocks();
  });

  it("should render the register car form", () => {
    const brandsServiceSpy = vi.spyOn(brandServices, "getAllBrands");

    render(
      <BrowserRouter>
        <RegisterCarForm />
      </BrowserRouter>
    );

    expect(screen.getByText(/nome \/ descrição/i)).toBeInTheDocument();
    expect(screen.getByText(/modelo/i)).toBeInTheDocument();
    expect(screen.getByText(/^marca$/i)).toBeInTheDocument();
    expect(screen.getByText(/ano/i)).toBeInTheDocument();
    expect(screen.getByText(/preço/i)).toBeInTheDocument();
    expect(screen.getByText(/imagem/i)).toBeInTheDocument();

    expect(
      screen.getByRole("button", { name: /cadastrar nova marca/i })
    ).toBeInTheDocument();

    expect(
      screen.getByRole("button", { name: /^cadastrar$/i })
    ).toBeInTheDocument();
    expect(
      screen.getByRole("button", { name: /^cancelar$/i })
    ).toBeInTheDocument();

    expect(brandsServiceSpy).toHaveBeenCalled();
  });

  it("should render the edit car form", () => {
    vi.spyOn(router, "useLocation").mockReturnValueOnce({
      state: {
        editMode: true,
        car: carMock,
      },
      hash: "",
      key: "",
      pathname: "",
      search: "",
    });

    const brandsServiceSpy = vi.spyOn(brandServices, "getAllBrands");

    render(
      <BrowserRouter>
        <RegisterCarForm />
      </BrowserRouter>
    );

    expect(screen.getByText(/nome \/ descrição/i)).toBeInTheDocument();
    expect(screen.getByText(/modelo/i)).toBeInTheDocument();
    expect(screen.getByText(/^marca$/i)).toBeInTheDocument();
    expect(screen.getByText(/ano/i)).toBeInTheDocument();
    expect(screen.getByText(/preço/i)).toBeInTheDocument();
    expect(screen.getByText(/imagem/i)).toBeInTheDocument();

    expect(
      screen.getByRole("button", { name: /cadastrar nova marca/i })
    ).toBeInTheDocument();

    expect(
      screen.getByRole("button", { name: /^editar$/i })
    ).toBeInTheDocument();
    expect(
      screen.getByRole("button", { name: /^cancelar$/i })
    ).toBeInTheDocument();

    expect(brandsServiceSpy).toHaveBeenCalled();
  });
});
