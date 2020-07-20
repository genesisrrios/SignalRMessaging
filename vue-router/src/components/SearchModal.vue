<template>
<!-- Modal -->
<div class="modal fade" id="modalComponent" tabindex="-1" role="dialog" aria-labelledby="exampleModalLabel" aria-hidden="true">
  <div class="modal-dialog" role="document">
    <div v-bind:style="{'background-color':primaryColor}"  class="modal-content">
      <div class="modal-header" v-bind:style="{'background-color':primaryColor}">
        <h5 class="modal-title" id="exampleModalLabel">Search username</h5>
          <button type="button" class="close" data-dismiss="modal" aria-label="Close">
            <span aria-hidden="true">&times;</span>
          </button>
      </div>
      <div class="modal-body">
        <form>
          <div class="form-group">
            <label for="searchUsername">Username</label>
            <input type="text" class="form-control" id="searchUsername" placeholder="Search contact by username" v-model="searchTerm">
          </div>
          <select name="users-in-search" id="users-in-search" v-model="selectedUserToAdd">
            <option v-for="user in userListBySearchTerm" value="user.id" v-bind:key="user.user_name">{{user.user_name}}</option>
          </select>
        </form>
      </div>
      <div class="modal-footer" v-bind:style="{'background-color':primaryColor}">
        <button type="button" class="btn btn-secondary" data-dismiss="modal">Close</button>
        <button type="button" class="btn btn-primary" v-on:click=AddContact>Add Contact</button>
      </div>
    </div>
  </div>
 </div>
</template>

<script>
import mixins from '../mixins.js'
import axios from 'axios'

export default {
  name: 'searchmodal',
  props: ['primaryColor'],
  mixins: [mixins],
  components: {
  },
  data () {
    return {
      color: this.primaryColor,
      searchTerm: null,
      userListBySearchTerm: [],
      selectedUserToAdd: null
    }
  },
  mounted () {
  },
  methods: {
    searchContactByName: function () {
      let self = this
      axios.get(`${this.apiUrl}api/user/searchContactByName?user_id=${localStorage.userId}&contact_name=${this.searchTerm}`)
        .then(function (response) {
          console.log(response.data.Values)
          self.userListBySearchTerm = response.data.Values
        }).catch(error => {
          console.log(error.response)
        })
    },
    AddContact: function () {
      console.log('test')
    }
  },
  watch: {
    searchTerm: function () {
      if (this.searchTerm && this.searchTerm.length > 3) {
        this.searchContactByName()
      }
    }
  }
}
</script>

<style>
</style>
