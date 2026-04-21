"use client";

import { useEffect } from "react";

const NAV_ID = "main-navbar";

export default function NavScrollBehavior() {
  useEffect(() => {
    const nav = document.getElementById(NAV_ID);
    if (!nav) return;

    const updateBodyOffset = () => {
      document.documentElement.style.setProperty("--nav-height", `${nav.offsetHeight}px`);
    };

    let lastScrollY = window.scrollY;
    const threshold = 8;

    const onScroll = () => {
      const currentScrollY = window.scrollY;

      if (currentScrollY <= threshold) {
        nav.classList.remove("nav-hidden");
      } else if (currentScrollY > lastScrollY + 2) {
        nav.classList.add("nav-hidden");
      } else if (currentScrollY < lastScrollY - 2) {
        nav.classList.remove("nav-hidden");
      }

      lastScrollY = currentScrollY;
    };

    updateBodyOffset();
    window.addEventListener("resize", updateBodyOffset);
    window.addEventListener("scroll", onScroll, { passive: true });

    return () => {
      window.removeEventListener("resize", updateBodyOffset);
      window.removeEventListener("scroll", onScroll);
    };
  }, []);

  return null;
}
