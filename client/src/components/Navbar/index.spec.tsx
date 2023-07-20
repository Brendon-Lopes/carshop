import "@testing-library/jest-dom";
import { render } from "@testing-library/react";
import { BrowserRouter as Router } from "react-router-dom";
import { describe, expect, it } from "vitest";
import { Navbar } from ".";

describe("Navbar component", () => {
  it("should render navbar text", () => {
    const sut = render(
      <Router>
        <Navbar />
      </Router>
    );

    expect(sut.getByText("Carshop")).toBeInTheDocument();
    expect(sut.getByText("Login")).toBeInTheDocument();
  });
});
