import type { NextConfig } from "next";

const nextConfig: NextConfig = {
  experimental: {
    serverActions: {
      bodySizeLimit: '5mb', // Set to 5MB
    },
  },
};

export default nextConfig;
