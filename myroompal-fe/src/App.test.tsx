import React from 'react';
import { render, screen } from '@testing-library/react';
import Home from './pages/home';
import App from './App';
import { MemoryRouter } from 'react-router-dom';


test('renders learn react link', () => {
  render(
    <MemoryRouter>
    <Home />
  </MemoryRouter>
  );
  const linkElement = screen.getByText(/Newly available rooms/);
  expect(linkElement).toBeInTheDocument();
});
