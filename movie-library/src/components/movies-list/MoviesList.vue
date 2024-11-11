<template>
	<div>
		<create-movie
			@movie-added="fetchMovies"
			@add-error="showErrorF"
			@fetch-movies-from-api="fetchMovies"
		></create-movie>
		<ul class="list-group mt-3 mb-3">
			<movie-list-item
				v-for="movie in movies.movies"
				:key="movie.id"
				:movie="movie"
				@deleteConfirm="deleteConfirmF"
				@editMovie="editMovieF"
			></movie-list-item>
		</ul>
	</div>
	<!-- #region DeleteModal -->
	<div
		class="modal fade"
		id="deleteMovieModal"
		tabindex="-1"
		aria-labelledby="deleteMovieModalLabel"
		aria-hidden="true"
	>
		<div class="modal-dialog">
			<div class="modal-content">
				<div class="modal-header">
					<h5
						class="modal-title"
						id="deleteMovieModalLabel"
					>
						Usuń film
					</h5>
					<button
						type="button"
						class="btn-close"
						data-bs-dismiss="modal"
						aria-label="Close"
					></button>
				</div>
				<div class="modal-body">
					Czy na pewno chcesz usunąć wybrany film?
				</div>
				<div class="modal-footer">
					<button
						type="button"
						class="btn btn-secondary"
						data-bs-dismiss="modal"
					>
						Zamknij
					</button>
					<button
						type="button"
						class="btn btn-danger"
						data-bs-dismiss="modal"
						@click="deleteMovie()"
					>
						Usuń
					</button>
				</div>
			</div>
		</div>
	</div>
	<!-- #endregion -->
	<edit-movie
		ref="editMovieRef"
		@movie-edited="fetchMovies"
		@edit-error="showErrorF"
	></edit-movie>
	<teleport to="body">
		<error-dialog ref="errorDialogRef"></error-dialog>
	</teleport>
</template>

<script setup>
	import axios from 'axios';
	import { ref, onMounted, inject } from 'vue';
	import MovieListItem from './MovieListItem.vue';
	import CreateMovie from '../CreateMovie.vue';
	import EditMovie from '../EditMovie.vue';
	import ErrorDialog from '../ErrorDialog.vue';

	const editMovieRef = ref(null);
	const errorDialogRef = ref(null);
	const bootstrap = inject('bootstrap');
	const deletedID = ref(null);
	const movies = ref([]);

	const fetchMovies = async () => {
		await axios
			.get('https://localhost:7009/movies')
			.then(response => {
				movies.value = response.data;
			})
			.catch(error => {
				console.log(error);
				showError(error);
			});
	};

	const deleteMovie = async () => {
		await axios
			.delete(`https://localhost:7009/movies/${deletedID.value}`)
			.then(() => {
				deletedID.value = null;
				fetchMovies();
			})
			.catch(error => {
				console.log(error);
				showError(error);
			});
	};

	const showErrorF = error => {
		showError(error);
	};

	let modalInstance;
	const deleteConfirmF = id => {
		id ? (deletedID.value = id) : '';

		const modalElement = document.getElementById('deleteMovieModal');
		if (modalElement) {
			modalInstance = new bootstrap.Modal(modalElement);
			modalInstance.show();
		} else {
			console.error('Modal element not found');
		}
	};

	const editMovieF = id => {
		editMovieRef.value.editMovie(
			movies.value.movies.find(movie => movie.id == id)
		);
	};

	const showError = err => {
		errorDialogRef.value.showErrorDialog([err.code, err.message]);
	};

	onMounted(() => {
		fetchMovies();
	});
</script>
