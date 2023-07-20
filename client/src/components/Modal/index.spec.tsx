import "@testing-library/jest-dom";
import { render, screen } from "@testing-library/react";
import { describe, expect, it } from "vitest";
import { Modal } from ".";

describe("Modal component", () => {
  it("should render the modal", () => {
    render(
      <Modal isOpen>
        <h1>Modal</h1>
      </Modal>
    );

    expect(
      screen.getByRole("heading", { name: /modal/i, level: 1 })
    ).toBeInTheDocument();
  });
});
