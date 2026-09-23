import { Outlet } from "react-router-dom";
import NavMenu from "./NavMenu";

export default function Layout() {
  return (
    <div className="page d-flex flex-column min-vh-100">
      <NavMenu />
      <main className="container flex-grow-1">
        <Outlet />
      </main>
      <footer className="text-center py-3 text-muted border-top mt-5">
        <small>© 2026 Mainų platformos projektas. Visos teisės saugomos.</small>
      </footer>
    </div>
  );
}