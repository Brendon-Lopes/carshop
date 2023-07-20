import "@testing-library/jest-dom";
import { render, screen } from "@testing-library/react";
import { describe, expect, it, vi } from "vitest";
import { BrowserRouter as Router } from "react-router-dom";
import { CarCard } from ".";
import * as cookie from "react-cookie";
import { UserRoles } from "src/enums";
import { carMock } from "src/__tests__/mocks/carMock";

describe("CarCard component", () => {
  vi.mock("react-cookie", () => ({
    useCookies: vi.fn().mockReturnValue([{}]),
  }));

  afterAll(() => {
    vi.restoreAllMocks();
  });

  it("should render the navbar", () => {
    render(
      <Router>
        <CarCard car={carMock} onCarDelete={() => null} />
      </Router>
    );

    expect(screen.getByText(/audi a3/i)).toBeInTheDocument();
    expect(screen.getByText(/audi - a3 - ano 2021/i)).toBeInTheDocument();
    expect(screen.getByText(/r\$ 100.000,00/i)).toBeInTheDocument();

    expect(screen.getByRole("img")).toBeInTheDocument();
  });

  it("should render the edit and delete buttons when user is Admin", () => {
    vi.spyOn(cookie, "useCookies").mockReturnValue([
      { role: UserRoles.Admin, token: "123456" },
      () => null,
      () => null,
    ]);

    render(
      <Router>
        <CarCard car={carMock} onCarDelete={() => null} />
      </Router>
    );

    expect(screen.getByTestId("edit-button")).toBeInTheDocument();
    expect(screen.getByTestId("delete-button")).toBeInTheDocument();
  });

  it("should not render the edit and delete buttons when user is Customer", () => {
    vi.spyOn(cookie, "useCookies").mockReturnValue([
      { role: UserRoles.Customer, token: "123456" },
      () => null,
      () => null,
    ]);

    render(
      <Router>
        <CarCard car={carMock} onCarDelete={() => null} />
      </Router>
    );

    expect(screen.queryByTestId("edit-button")).not.toBeInTheDocument();
  });
});
