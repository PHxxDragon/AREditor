namespace EAR.Entity
{
    public class VisibleEntity : BaseEntity
    {
        protected bool isVisible = true;

        public override void ResetEntityState()
        {
            base.ResetEntityState();
            gameObject.SetActive(true);
        }

        public override void StartDefaultState()
        {
            base.StartDefaultState();
            gameObject.SetActive(isVisible);
        }

        public override bool IsViewable()
        {
            return true;
        }
        public override bool IsClickable()
        {
            return true;
        }
    }
}

