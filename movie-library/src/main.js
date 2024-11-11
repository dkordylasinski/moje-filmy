import { createApp } from 'vue';
import App from './App.vue';
import * as bootstrap from 'bootstrap/dist/js/bootstrap.bundle';

const app = createApp(App);

app.provide('bootstrap', bootstrap);
app.mount('#app');
