// App.test.jsx
import { render, screen, fireEvent, waitFor } from '@testing-library/react';
import { describe, it, expect, vi, beforeEach } from 'vitest';
import App from './App';

// Mock fetch for testing
globalThis.fetch = vi.fn();

// now you can access it as `IntersectionObserver` or `window.IntersectionObserver`
describe('App Component', () => {
  beforeEach(() => {
    fetch.mockClear();
  });

  // TC: 1
  it('renders the heading and form', () => {
    render(<App />);
    expect(screen.getByText(/Conversion Numbers/i)).toBeInTheDocument();
    expect(screen.getByLabelText(/Type a number/i)).toBeInTheDocument();
    expect(screen.getByRole('button', { name: /Convert/i })).toBeInTheDocument();
  });

  // TC: 2
  it('shows error when input is not a number', () => {
    render(<App />);
    const input = screen.getByLabelText(/Type a number/i);
    fireEvent.change(input, { target: { value: 'abc' } });
    expect(screen.getByText(/Please enter a valid number./i)).toBeInTheDocument();
  });

  // TC: 3
  it('shows error when number is greater than a billion', () => {
    render(<App />);
    const input = screen.getByLabelText(/Type a number/i);
    fireEvent.change(input, { target: { value: '1000000000' } });
    fireEvent.click(screen.getByRole('button', { name: /Convert/i }));
    expect(screen.getByText(/Please enter a valid number less than a billion. or with two decimal places./i)).toBeInTheDocument();
  });

  // TC: 4
  it('handles valid form submission', async () => {
    fetch.mockResolvedValueOnce({
      ok: true,
      json: async () => [{ id: 1, dollars: 'ONE HUNDRED', cents: '' }],
    });

    render(<App />);
    const input = screen.getByLabelText(/Type a number/i);
    fireEvent.change(input, { target: { value: '100' } });
    fireEvent.click(screen.getByRole('button', { name: /Convert/i }));

    await waitFor(() => {
      expect(screen.queryByText(/Please enter a valid number less than a billion. or with two decimal places./i)).not.toBeInTheDocument();
      expect(screen.queryByText(/Please enter a valid number./i)).not.toBeInTheDocument();
      expect(screen.getByText(/ONE HUNDRED DOLLARS/i)).toBeInTheDocument();
    });
  });

  // TC: 5
  it('handles fetch error', async () => {
    fetch.mockRejectedValueOnce(new Error('API is down'));

    render(<App />);
    const input = screen.getByLabelText(/Type a number/i);
    fireEvent.change(input, { target: { value: '100.00' } });
    fireEvent.click(screen.getByRole('button', { name: /Convert/i }));

    await waitFor(() => {
      expect(screen.getByText(/An error occurred while converting the number./i)).toBeInTheDocument();
    });
  });
});