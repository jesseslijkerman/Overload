/**
 * Base path of the backend API. Relative on purpose: in Docker nginx forwards /api to the API
 * container and 'ng serve' forwards it through proxy.conf.json, so no CORS setup is needed.
 */
export const API_BASE_URL = '/api';
