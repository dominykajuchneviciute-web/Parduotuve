import { createContext, useContext, useEffect, useState } from "react";
import type { ReactNode } from "react";

const API = "http://localhost:5145/api/auth";

type User = { email: string };

type AuthContextType = {
  user: User | null;
  loading: boolean;
  login: (email: string, password: string) => Promise<string | null>;
  register: (email: string, password: string) => Promise<string | null>;
  logout: () => Promise<void>;
};

const AuthContext = createContext<AuthContextType | null>(null);

const errorMessages: Record<string, string> = {
  DuplicateUserName: "Vartotojas su tokiu el. paštu jau egzistuoja.",
  DuplicateEmail: "Vartotojas su tokiu el. paštu jau egzistuoja.",
  InvalidEmail: "Neteisingas el. pašto formatas.",
  PasswordTooShort: "Slaptažodis turi būti bent 6 simbolių ilgio.",
  PasswordRequiresDigit: "Slaptažodyje turi būti bent vienas skaitmuo.",
  PasswordRequiresLower: "Slaptažodyje turi būti bent viena mažoji raidė.",
  PasswordRequiresUpper: "Slaptažodyje turi būti bent viena didžioji raidė.",
  PasswordRequiresNonAlphanumeric: "Slaptažodyje turi būti bent vienas simbolis (pvz., !).",
};

export function AuthProvider({ children }: { children: ReactNode }) {
  const [user, setUser] = useState<User | null>(null);
  const [loading, setLoading] = useState(true);

  async function loadUser() {
    try {
      const res = await fetch(`${API}/manage/info`, { credentials: "include" });
      if (res.ok) {
        const data = await res.json();
        setUser({ email: data.email });
      } else {
        setUser(null);
      }
    } catch {
      setUser(null);
    } finally {
      setLoading(false);
    }
  }

  useEffect(() => {
    loadUser();
  }, []);

  async function login(email: string, password: string) {
    try {
      const res = await fetch(`${API}/login?useCookies=true`, {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        credentials: "include",
        body: JSON.stringify({ email, password }),
      });
      if (!res.ok) {
        return "Neteisingas el. paštas arba slaptažodis.";
      }
      await loadUser();
      return null;
    } catch {
      return "Nepavyko prisijungti prie serverio.";
    }
  }

  // Returns error if registration failed, null if it worked
  async function register(email: string, password: string) {
    try {
      const res = await fetch(`${API}/register`, {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        credentials: "include",
        body: JSON.stringify({ email, password }),
      });
      if (!res.ok) {
        const data = await res.json().catch(() => null);
        const codes = data?.errors ? Object.keys(data.errors) : [];
        if (codes.length > 0) {
          return codes
            .map((code) => errorMessages[code] ?? data.errors[code][0])
            .join(" ");
        }
        return "Registracija nepavyko.";
      }
      // Registration worked
      return await login(email, password);
    } catch {
      return "Nepavyko prisijungti prie serverio.";
    }
  }

  async function logout() {
    try {
      await fetch(`${API}/logout`, {
        method: "POST",
        credentials: "include",
      });
    } finally {
      setUser(null);
    }
  }

  return (
    <AuthContext.Provider value={{ user, loading, login, register, logout }}>
      {children}
    </AuthContext.Provider>
  );
}

export function useAuth() {
  const ctx = useContext(AuthContext);
  if (!ctx) throw new Error("useAuth must be used inside AuthProvider");
  return ctx;
}