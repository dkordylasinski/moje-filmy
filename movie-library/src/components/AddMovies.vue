<template>
	<button
		type="button"
		class="btn btn-primary d-block"
		@click="fetchMoviesFromApi"
	>
		Pobierz filmy
	</button>
	<teleport to="body">
		<error-dialog ref="errorDialogRef"></error-dialog>
	</teleport>
</template>

<script setup>
	import axios from 'axios';
	import { defineEmits, ref } from 'vue';
	import ErrorDialog from './ErrorDialog.vue';
	const errorDialogRef = ref(null);

	const emit = defineEmits(['fetch-movies-from-api']);

	const saveNewMovies = async movies => {
		console.log(movies);
		await axios
			.post('https://localhost:7009/movies/bulk', movies)
			.then(() => {
				emit('fetch-movies-from-api');
			})
			.catch(error => {
				console.log(error);
				errorDialogRef.value.showErrorDialog([
					error.code,
					error.message,
				]);
			});
	};

	const fetchMoviesFromApi = async () => {
		await axios
			.get('/api/MyMovies')
			.then(response => {
				saveNewMovies(response.data);
			})
			.catch(error => {
				console.log(error);
				errorDialogRef.value.showErrorDialog([
					error.code,
					error.message,
				]);
			});
	};
</script>
