import "@testing-library/jest-dom";
import { render, screen } from "@testing-library/react";
import { afterEach, describe, expect, it, vi } from "vitest";
import { SearchBar } from ".";

describe("SearchBar component", () => {
  afterEach(() => {
    vi.restoreAllMocks();
  });

  it("should render the search bar", () => {
    render(<SearchBar onSearch={vi.fn()} />);

    expect(screen.getByRole("textbox")).toBeInTheDocument();
  });

  it("should call the onSearch function when the form is submitted", () => {
    const onSearch = vi.fn();

    render(<SearchBar onSearch={onSearch} />);

    screen.getByTestId("submit-btn").click();

    expect(onSearch).toHaveBeenCalledOnce();
  });
});
