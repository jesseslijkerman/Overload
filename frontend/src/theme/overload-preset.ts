import { definePreset } from '@primeuix/themes';
import Aura from '@primeuix/themes/aura';

/**
 * Overload theme preset: the only place where raw hex colors live.
 * Everything else in the app uses CSS variables (--p-* from PrimeNG, --ov-* from styles.scss).
 */
export const OverloadPreset = definePreset(Aura, {
  // The cast is needed because PrimeNG's Primitive type does not allow custom palette names
  primitive: {
    // ColorBrewer "Greens" ramp -> generates --p-overload-50 ... --p-overload-800
    overload: {
      50: '#F7FCF5',
      100: '#E5F5E0',
      200: '#C7E9C0',
      300: '#A1D99B',
      400: '#74C476',
      500: '#41AB5D',
      600: '#238B45',
      700: '#006D2C',
      800: '#00441B',
    },
  } as object,
  semantic: {
    primary: {
      50: '{overload.50}',
      100: '{overload.100}',
      200: '{overload.200}',
      300: '{overload.300}',
      400: '{overload.400}',
      500: '{overload.500}',
      600: '{overload.600}',
      700: '{overload.700}',
      800: '{overload.800}',
      900: '{overload.800}',
      950: '{overload.800}',
    },
    colorScheme: {
      light: {
        // Green-tinted neutrals for the light theme
        surface: {
          0: '#FFFFFF',
          50: '#F7FCF5',
          100: '#E5F5E0',
          200: '#C7E9C0',
          300: '#A9C7B0',
          400: '#7FA08A',
          500: '#4F7059',
          600: '#3E5A46',
          700: '#2A4333',
          800: '#143020',
          900: '#0A2315',
          950: '#07170D',
        },
        primary: {
          color: '{overload.700}', // 6.3:1 on the light page background
          contrastColor: '#F7FCF5',
          hoverColor: '{overload.800}',
          activeColor: '{overload.800}',
        },
      },
      dark: {
        // Green-tinted neutrals for the dark (gym) theme: 950 = page, 900 = cards
        surface: {
          0: '#FFFFFF',
          50: '#F7FCF5',
          100: '#E5F5E0',
          200: '#E5F5E0',
          300: '#C7E9C0',
          400: '#B6D4BD',
          500: '#7FA08A',
          600: '#3A5A45',
          700: '#1E3D2A',
          800: '#16301F',
          900: '#0E2517',
          950: '#07170D',
        },
        primary: {
          color: '{overload.400}', // 8.7:1 on the dark page background
          contrastColor: '#07170D',
          hoverColor: '{overload.300}',
          activeColor: '{overload.200}',
        },
      },
    },
  },
  components: {
    button: {
      root: {
        borderRadius: '8px',
        paddingY: '0.75rem', // keeps buttons at least 44px tall
      },
    },
  },
});
