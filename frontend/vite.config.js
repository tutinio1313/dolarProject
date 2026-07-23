import { defineConfig } from 'vite';
import angular from 'vite-plugin-angular';
import tailwindcss from '@tailwindcss/vite';

export default defineConfig({
  plugins: [angular(), tailwindcss()],
  server: {
    port: 4200,
  },
});