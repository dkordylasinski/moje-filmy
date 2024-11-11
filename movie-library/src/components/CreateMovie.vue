<template>
	<div class="d-flex justify-content-between">
		<add-movies
			@fetch-movies-from-api="emit('fetch-movies-from-api')"
		></add-movies>
		<button
			type="button"
			class="btn btn-primary ms-auto d-block"
			@click="modalState.show()"
		>
			Dodaj filmy
		</button>
	</div>
	<div
		class="modal fade"
		id="createMovie"
		tabindex="-1"
		aria-labelledby="createMovieLabel"
		aria-hidden="true"
	>
		<div class="modal-dialog">
			<div class="modal-content">
				<div class="modal-header">
					<h5
						class="modal-title"
						id="createMovieLabel"
					>
						Dodaj film
					</h5>
					<button
						type="button"
						class="btn-close"
						data-bs-dismiss="modal"
						aria-label="Close"
					></button>
				</div>
				<div class="modal-body">
					<div class="mb-3">
						<label
							for="movieTitle"
							class="form-label"
							>Tytuł Filmu</label
						>
						<input
							v-model="state.title"
							type="text"
							class="form-control"
							:class="{
								'is-invalid': v$.title?.$errors.length > 0,
							}"
							id="movieTitle"
							placeholder="Wpisz tytuł filmu"
							aria-describedby="movieTitleFeedback"
						/>
						<div
							id="movieTitleFeedback"
							class="invalid-feedback"
						>
							<span
								v-if="
									v$.title?.$errors.some(
										error =>
											error.$validator === 'maxLength'
									)
								"
							>
								Tytuł może mieć maksymalnie 200 znaków.
							</span>
							<span
								v-else-if="
									v$.title?.$errors.some(
										error => error.$validator === 'required'
									)
								"
							>
								Proszę wpisać tytuł filmu.
							</span>
						</div>
					</div>

					<div class="mb-3">
						<label
							for="releaseYear"
							class="form-label"
							>Rok Utworzenia</label
						>
						<input
							v-model="state.year"
							type="number"
							class="form-control"
							:class="{
								'is-invalid': v$.year?.$errors.length > 0,
							}"
							id="releaseYear"
							aria-describedby="movieYearFeedback"
							placeholder="Wpisz rok utworzenia filmu"
						/>
						<div
							id="movieYearFeedback"
							class="invalid-feedback"
						>
							<span
								v-if="
									v$.year?.$errors.some(
										error => error.$validator === 'between'
									)
								"
							>
								Rok utworzenia powinien znajdować się w
								przedziale 1900 a 2200.
							</span>
							<span
								v-else-if="
									v$.year?.$errors.some(
										error => error.$validator === 'required'
									)
								"
							>
								Proszę wpisać rok produkcji filmu.
							</span>
						</div>
					</div>

					<div class="mb-3">
						<label
							for="director"
							class="form-label"
							>Reżyser</label
						>
						<input
							v-model="state.director"
							type="text"
							class="form-control"
							id="director"
							placeholder="Wpisz imię i nazwisko reżysera"
						/>
					</div>

					<div class="mb-3">
						<label
							for="description"
							class="form-label"
							>Opis</label
						>
						<textarea
							v-model="state.description"
							class="form-control"
							id="description"
							rows="3"
							placeholder="Wpisz opis filmu"
						></textarea>
					</div>
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
						class="btn btn-success"
						@click="checkValidation"
					>
						Dodaj
					</button>
				</div>
			</div>
		</div>
	</div>
</template>

<script setup>
	import axios from 'axios';
	import { ref, onMounted, inject, defineEmits } from 'vue';
	import { useVuelidate } from '@vuelidate/core';
	import { required, maxLength, between } from '@vuelidate/validators';

	import AddMovies from './AddMovies.vue';

	const state = ref({
		title: '',
		year: 0,
		director: '',
		description: '',
	});
	const rules = ref({
		title: { required, maxLength: maxLength(200) },
		year: { required, between: between(1900, 2200) },
	});
	const v$ = useVuelidate(rules, state);
	const clearForm = () => {
		v$.value.$reset();
		state.value.title = '';
		state.value.year = 0;
		state.value.director = '';
		state.value.description = '';
	};
	const emit = defineEmits([
		'movie-added',
		'add-error',
		'fetch-movies-from-api',
	]);
	const addMovie = async () => {
		await axios
			.post('https://localhost:7009/movies', state.value)
			.then(() => {
				emit('movie-added');
			})
			.catch(error => {
				console.log(error);
				emit('add-error', error);
			});
	};

	const bootstrap = inject('bootstrap');
	const modalState = ref(null);
	onMounted(() => {
		modalState.value = new bootstrap.Modal('#createMovie', {});
	});

	const checkValidation = () => {
		v$.value.$touch();
		if (!v$.value.$invalid) {
			modalState.value.hide();
			addMovie();

			clearForm();
		}
	};
</script>
