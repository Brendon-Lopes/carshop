import "@testing-library/jest-dom";
import { render, screen } from "@testing-library/react";
import { afterEach, describe, expect, it, vi } from "vitest";
import { ConfirmationModal } from ".";

describe("ConfirmationModal component", () => {
  afterEach(() => {
    vi.restoreAllMocks();
  });

  it("should render the confirmation modal", () => {
    render(
      <ConfirmationModal
        isOpen
        onConfirmation={() => null}
        onCancel={() => null}
      >
        <h1>Modal</h1>
      </ConfirmationModal>
    );

    expect(
      screen.getByRole("heading", { name: /modal/i, level: 1 })
    ).toBeInTheDocument();
  });

  it("should call the onConfirmation function when the confirmation button is clicked", () => {
    const onConfirmation = vi.fn();

    render(
      <ConfirmationModal
        isOpen
        onConfirmation={onConfirmation}
        onCancel={() => null}
      >
        <h1>Modal</h1>
      </ConfirmationModal>
    );

    screen.getByRole("button", { name: /confirmar/i }).click();

    expect(onConfirmation).toHaveBeenCalledOnce();
  });

  it("should call the onCancel function when the cancel button is clicked", () => {
    const onCancel = vi.fn();

    render(
      <ConfirmationModal isOpen onConfirmation={() => null} onCancel={onCancel}>
        <h1>Modal</h1>
      </ConfirmationModal>
    );

    screen.getByRole("button", { name: /cancelar/i }).click();

    expect(onCancel).toHaveBeenCalledOnce();
  });
});
