<template>
	<div
		class="modal fade"
		id="editMovie"
		tabindex="-1"
		aria-labelledby="editMovieLabel"
		aria-hidden="true"
	>
		<div class="modal-dialog">
			<div class="modal-content">
				<div class="modal-header">
					<h5
						class="modal-title"
						id="editMovieLabel"
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
							v-model="editFields.title"
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
							v-model="editFields.year"
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
							v-model="editFields.director"
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
							v-model="editFields.description"
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
						@click="checkValidation()"
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
	import { ref, inject, defineExpose, defineEmits } from 'vue';
	import { useVuelidate } from '@vuelidate/core';
	import { required, maxLength, between } from '@vuelidate/validators';

	const currentID = ref(null);
	const editFields = ref({
		title: '',
		year: '',
		director: '',
		description: '',
	});
	const rules = ref({
		title: { required, maxLength: maxLength(200) },
		year: { required, between: between(1900, 2200) },
	});
	const v$ = useVuelidate(rules, editFields);
	const clearForm = () => {
		v$.value.$reset();
		editFields.value.title = '';
		editFields.value.year = 0;
		editFields.value.director = '';
		editFields.value.description = '';
	};

	const bootstrap = inject('bootstrap');
	const modalState = ref(null);

	const editMovie = movie => {
		modalState.value = new bootstrap.Modal('#editMovie', {});
		modalState.value.show();

		currentID.value = movie.id;
		editFields.value.title = movie.title;
		editFields.value.year = movie.year;
		editFields.value.director = movie.director;
		editFields.value.description = movie.description;
	};

	const emit = defineEmits(['movie-edited', 'edit-error']);
	const updateMovie = async id => {
		await axios
			.put(`https://localhost:7009/movies/${id}`, editFields.value)
			.then(() => {
				emit('movie-edited');
				modalState.value.hide();
			})
			.catch(error => {
				console.log(error);
				emit('edit-error', error);
			});
	};

	const checkValidation = () => {
		v$.value.$touch();
		if (!v$.value.$invalid) {
			modalState.value.hide();
			updateMovie(currentID.value);
			clearForm();
		}
	};

	defineExpose({
		editMovie,
	});
</script>

<style lang="scss" scoped></style>
