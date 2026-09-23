import { NavLink } from "react-router-dom";

export default function NavMenu() {
  return (
    <header className="navbar navbar-expand-lg navbar-dark bg-dark px-4 py-2 mb-4 shadow-sm">
      <div className="container-fluid d-flex justify-content-between align-items-center">
        <NavLink className="navbar-brand fw-bold fs-4 text-white text-decoration-none" to="/">
          Mainų platforma
        </NavLink>

        <nav className="d-flex gap-3">
          <NavLink 
            to="/" 
            className={({ isActive }) => `nav-link px-3 py-1 rounded ${isActive ? "active text-white" : "text-white-50"}`}
          >
            Pradžia
          </NavLink>
          <NavLink 
            to="/items" 
            className={({ isActive }) => `nav-link px-3 py-1 rounded ${isActive ? "active text-white" : "text-white-50"}`}
          >
            Daiktai
          </NavLink>
          <NavLink 
            to="/my-items" 
            className={({ isActive }) => `nav-link px-3 py-1 rounded ${isActive ? "active text-white" : "text-white-50"}`}
          >
            Mano daiktai
          </NavLink>
          <NavLink 
            to="/login" 
            className={({ isActive }) => `nav-link px-3 py-1 rounded ${isActive ? "active text-white" : "text-white-50"}`}
          >
            Prisijungti
          </NavLink>
        </nav>
      </div>

      <style>{`
        .nav-link.active {
          color: #ffffff !important;
          background-color: rgba(255, 255, 255, 0.15);
          font-weight: 600;
        }
        .nav-link:hover {
          color: #ffffff !important;
        }
      `}</style>
    </header>
  );
}